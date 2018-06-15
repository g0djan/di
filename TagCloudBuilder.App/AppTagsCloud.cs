﻿using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Autofac;
using TagCloudBuilder.Domain;
using TagCloudBuilder.Infrastructure;

namespace TagCloudBuilder.App
{
    public partial class AppTagCloud : Form
    {
        private const int HeightBtwFields = 35;
        private const int HeightBtwLabelAndField = 15;
        private const int WidthBtwFields = 5;
        private static readonly HashSet<string> boringWords = new HashSet<string>();

        private Result<int> Width => int.TryParse(WidthBox.Text, out var n) && n > 0
            ? Result.Ok(n)
            : Result.Fail<int>($"Incorrect width, try use number from interval [0;{Picture.Height}]");

        private Result<int> Height => int.TryParse(HeightBox.Text, out var n) && n > 0
            ? Result.Ok(n)
            : Result.Fail<int>($"Incorrect height, try use number from interval [0;{Picture.Height}]");


        public AppTagCloud(IContainer container)
        {
            InitializeComponent();
            DoubleBuffered = true;
            ClientSize = new Size(Picture.Location.X + Picture.Width, Picture.Height);
            MinimumSize = ClientSize;
            MaximumSize = ClientSize;

            AddToBoringWordsButton.Click += (sender, args) => boringWords.Add(BoringWordsTextBox.Text);
            RemoveToBoringWordsButton.Click += (sender, args) => boringWords.Remove(BoringWordsTextBox.Text);
            
            BuildButton.Click += (sender, args) =>
            {
                ColorsComboBox.SelectedIndex = ColorsComboBox.FindStringExact(ColorsComboBox.SelectedText);
                Picture.Image = null;
                var resultSettings = GetSettings();
                if (!resultSettings.IsSuccess)
                {
                    ErrorLabel.Text = resultSettings.Error;
                    return;
                }

                var settings = resultSettings.GetValueOrThrow();
                ErrorLabel.Text = "";
                var filter = container.ResolveNamed<IWordsFilter>(settings.WordsFilter);
                var reader = container.ResolveNamed<IFileReader>(settings.Reader);
                var parser = container.Resolve<ITextParser>();
                var editor = container.ResolveNamed<IWordsEditor>(settings.WordsEditor);
                var builder = container.Resolve<ITagCloudBuilder>();
                var drawer = container.ResolveNamed<ITextRectanglesDrawer>(settings.Drawer);
                var textRectangles = Build(filter, reader, parser, editor, builder, settings);
                if (!textRectangles.IsSuccess)
                {
                    ErrorLabel.Text = textRectangles.Error;
                    return;
                }
                var result = Result.OfAction(() => DrawTagCloud(drawer, textRectangles.GetValueOrThrow(), settings));
                if (!result.IsSuccess)
                {
                    ErrorLabel.Text = result.Error;
                    return;
                }
                using (var fs = new FileStream("cloud.png", FileMode.Open, FileAccess.Read))
                using (var original = Image.FromStream(fs))
                    Picture.Image = new Bitmap(original, 512, 512);
            };
            
            Controls.AddRange(new Control[]
            {
                FilenameBox, ImageFormatListBox, FileNameLabel, ImageFormatLabel,
                FontsListBox, FontsLabel,
                BoringWordsTextBox, AddToBoringWordsButton, RemoveToBoringWordsButton, BoringWordsLabel,
                BuildAlgorithmListBox, BuildAlgorithmLabel,
                WordsFormatListBox, WordsFormatLabel,
                PartOfSpeechListBox, PartsOfSpeechLabel,
                ColorsComboBox, ColorLabel,
                WidthLabel, WidthBox, HeightLabel, HeightBox,
                BuildButton,
                ErrorLabel,
                Picture
            });
        }

        private Result<IEnumerable<TextRectangle>> Build(IWordsFilter filter, IFileReader reader,
            ITextParser parser, IWordsEditor editor, ITagCloudBuilder builder, Settings settings)
        {
            filter.AddBoringWords(boringWords);
            return reader 
                .ReadFile(settings.InputPath)
                .Then(parser.GetWords) 
                .Then(filter.FilterWords)
                .Then(editor.Edit) 
                .Then(words => builder 
                    .GetTextRectangles(words, settings));
        }

        private void DrawTagCloud(ITextRectanglesDrawer drawer,
            IEnumerable<TextRectangle> textRectangles,
            Settings settings)
        {
            drawer.Draw(textRectangles, settings);
            drawer.Save(settings.Bitmap);
        }

        private Result<Settings> GetSettings()
        {
            if (!Width.IsSuccess)
                return Result.Fail<Settings>(Width.Error);
            if (!Height.IsSuccess)
                return Result.Fail<Settings>(Height.Error);
            var inputFileName = Path.Combine("..", "..", "Resources", FilenameBox.Text);
            var parts = FilenameBox.Text.Split('.');
            if (parts.Length != 2)
                return Result.Fail<Settings>("filename, should contain extension");
            return Result.Of(() =>
            {
                var color = Color.FromName((string) ColorsComboBox.SelectedItem);
                var fontFamily = FontFamily.Families[FontsListBox.SelectedIndex];
                var width = Width.GetValueOrThrow();
                var height = Height.GetValueOrThrow();
                var center = new Point(width / 2, height / 2);
                var bitmap = new Bitmap(width, height);
                var reader = parts[1];
                var filter = (string) PartOfSpeechListBox.SelectedItem;
                var editor = (string) WordsFormatListBox.SelectedItem;
                var layouter = (string) BuildAlgorithmListBox.SelectedItem;
                var drawer = (string) ImageFormatListBox.SelectedItem;
                var bounder = "WordsBounder";
                return new Settings(reader, filter, editor, layouter, drawer, bounder, color, fontFamily, center,
                    bitmap, inputFileName);
            });
        }


        private static Label FileNameLabel { get; } = new Label
        {
            Text = "Filename",
            Font = DefaultFont,
            Location = new Point(60, 100)
        };

        private static TextBox FilenameBox { get; } = new TextBox
        {
            Width = 200,
            Height = 29,
            Location = new Point(FileNameLabel.Location.X, FileNameLabel.Location.Y + HeightBtwLabelAndField),
            Text = "text.txt"
        };

        private static Label ImageFormatLabel { get; } = new Label
        {
            Text = "Format",
            Font = DefaultFont,
            Location = new Point(FileNameLabel.Location.X + FilenameBox.Width + WidthBtwFields,
                FileNameLabel.Location.Y)
        };

        private static ComboBox ImageFormatListBox { get; } = new ComboBox
        {
            Width = 50,
            Height = 29,
            Location = new Point(FilenameBox.Location.X + FilenameBox.Width + WidthBtwFields, FilenameBox.Location.Y),
            Items = {"png"},
            SelectedIndex = 0
        };

        private static Label BoringWordsLabel { get; } = new Label
        {
            Text = "Boring words",
            Font = DefaultFont,
            Location = new Point(FileNameLabel.Location.X, FilenameBox.Location.Y + HeightBtwFields)
        };

        private static TextBox BoringWordsTextBox { get; } = new TextBox
        {
            Width = 128,
            Height = 29,
            Location = new Point(BoringWordsLabel.Location.X, BoringWordsLabel.Location.Y + HeightBtwLabelAndField)
        };

        private static Button AddToBoringWordsButton { get; } = new Button
        {
            Text = "Add",
            Font = DefaultFont,
            Location = new Point(BoringWordsTextBox.Location.X + BoringWordsTextBox.Width + WidthBtwFields,
                BoringWordsTextBox.Location.Y),
            Width = 57,
            Height = 20
        };

        private static Button RemoveToBoringWordsButton { get; } = new Button
        {
            Text = "Remove",
            Font = DefaultFont,
            Location = new Point(AddToBoringWordsButton.Location.X + AddToBoringWordsButton.Width + WidthBtwFields,
                AddToBoringWordsButton.Location.Y),
            Width = 57,
            Height = 20
        };

        private static Label BuildAlgorithmLabel { get; } = new Label
        {
            Text = "Cloud shape",
            Font = DefaultFont,
            Location = new Point(BoringWordsTextBox.Location.X, BoringWordsTextBox.Location.Y + HeightBtwFields)
        };

        private static ComboBox BuildAlgorithmListBox { get; } = new ComboBox
        {
            Width = 128,
            Height = 29,
            Location = new Point(BuildAlgorithmLabel.Location.X,
                BuildAlgorithmLabel.Location.Y + HeightBtwLabelAndField),
            Items = {"Circular"},
            SelectedIndex = 0
        };

        private static Label WordsFormatLabel { get; } = new Label
        {
            Text = "Words formating",
            Font = DefaultFont,
            Location = new Point(AddToBoringWordsButton.Location.X, AddToBoringWordsButton.Location.Y + HeightBtwFields)
        };

        private static ComboBox WordsFormatListBox { get; } = new ComboBox
        {
            Width = 123,
            Height = 29,
            Location = new Point(WordsFormatLabel.Location.X, WordsFormatLabel.Location.Y + HeightBtwLabelAndField),
            Items = {"No format"},
            SelectedIndex = 0
        };

        private static Label PartsOfSpeechLabel { get; } = new Label
        {
            Text = "Choose part of speech",
            Width = 256,
            Font = DefaultFont,
            Location = new Point(BuildAlgorithmListBox.Location.X, BuildAlgorithmListBox.Location.Y + HeightBtwFields)
        };

        private static ComboBox PartOfSpeechListBox { get; } = new ComboBox
        {
            Width = 128,
            Height = 29,
            Location = new Point(PartsOfSpeechLabel.Location.X, PartsOfSpeechLabel.Location.Y + HeightBtwLabelAndField),
            Items = {"All"},
            SelectedIndex = 0
        };

        private static Label FontsLabel { get; } = new Label
        {
            Text = "Font",
            Font = DefaultFont,
            Location = new Point(PartOfSpeechListBox.Location.X + PartOfSpeechListBox.Width + WidthBtwFields,
                PartsOfSpeechLabel.Location.Y)
        };

        private static ComboBox FontsListBox
        {
            get
            {
                var fonts = new ComboBox
                {
                    Width = 123,
                    Height = 29,
                    Location = new Point(FontsLabel.Location.X, FontsLabel.Location.Y + HeightBtwLabelAndField)
                };
                foreach (var fontFamily in FontFamily.Families)
                    fonts.Items.Add(fontFamily.Name);
                fonts.SelectedIndex = 0;
                return fonts;
            }
        }

        private static Label ColorLabel { get; } = new Label
        {
            Text = "Color",
            Font = DefaultFont,
            Location = new Point(PartOfSpeechListBox.Location.X, PartOfSpeechListBox.Location.Y + HeightBtwFields)
        };

        private static ComboBox ColorsComboBox
        {
            get
            {
                var colors = new ComboBox
                {
                    Width = 100,
                    Height = 29,
                    FormattingEnabled = true,
                    Location = new Point(ColorLabel.Location.X, ColorLabel.Location.Y + HeightBtwLabelAndField)
                };
                foreach (var color in typeof(KnownColor).GetEnumNames())
                    colors.Items.Add(color);
                colors.SelectedIndex = 0;
                return colors;
            }
        }

        private static Label WidthLabel { get; } = new Label
        {
            Text = "Width",
            Width = 73,
            Height = 15,
            Font = DefaultFont,
            Location = new Point(ColorsComboBox.Location.X + ColorsComboBox.Width + WidthBtwFields,
                ColorLabel.Location.Y)
        };

        private static TextBox WidthBox { get; } = new TextBox
        {
            Width = 73,
            Height = 29,
            Location = new Point(WidthLabel.Location.X, WidthLabel.Location.Y + HeightBtwLabelAndField),
            Text = "1024"
        };

        private static Label HeightLabel { get; } = new Label
        {
            Text = "Height",
            Width = 73,
            Height = 15,
            Font = DefaultFont,
            Location = new Point(WidthBox.Location.X + WidthBox.Width + WidthBtwFields, ColorLabel.Location.Y)
        };

        private static TextBox HeightBox { get; } = new TextBox
        {
            Width = 73,
            Height = 29,
            Location = new Point(HeightLabel.Location.X, HeightLabel.Location.Y + HeightBtwLabelAndField),
            Text = "1024"
        };

        private static Button BuildButton { get; } = new Button
        {
            Text = "Build cloud",
            Font = DefaultFont,
            Location = new Point(ColorsComboBox.Location.X, ColorsComboBox.Location.Y + HeightBtwFields),
            Width = 256,
            Height = 30
        };

        private static Label ErrorLabel { get; } = new Label
        {
            Text = "",
            Font = DefaultFont,
            Location = new Point(BuildButton.Location.X, BuildButton.Location.Y + HeightBtwFields),
            Width = 256,
            Height = 50
        };

        private static PictureBox Picture { get; } = new PictureBox
        {
            Width = 512,
            Height = 512,
            Location = new Point(ImageFormatListBox.Location.X + ImageFormatListBox.Width + WidthBtwFields, 0)
        };
    }
}