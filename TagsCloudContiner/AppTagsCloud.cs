﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Autofac;
using IContainer = Autofac.IContainer;

namespace TagsCloudContainer
{
    public partial class AppTagsCloud : Form
    {
        private static readonly HashSet<string> boringWords = new HashSet<string>();
        private static List<RegistringImplemetation> registring = new List<RegistringImplemetation>();


        public AppTagsCloud()
        {
            InitializeComponent();
            DoubleBuffered = true;
            ClientSize = new Size(Picture.Location.X + Picture.Width, Picture.Height);

            AddToBoringWordsButton.Click += (sender, args) => boringWords.Add(BoringWordsTextBox.Text);
            RemoveToBoringWordsButton.Click += (sender, args) => boringWords.Remove(BoringWordsTextBox.Text);
            BuildButton.Click += (sender, args) =>
            {
                ColorsComboBox.SelectedIndex = ColorsComboBox.FindStringExact(ColorsComboBox.SelectedText);
                Picture.Image = null;
                var container = Program.SetContainer(GetSettings(), registring);
                Program.DrawTagsCloud(container, GetImplementationName(), boringWords);
                using (var fs = new FileStream("cloud.png", FileMode.Open, FileAccess.Read))
                using (var original = Image.FromStream(fs))
                    Picture.Image = new Bitmap(original);
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
                Picture
            });
        }

        private ImplementationName GetImplementationName()
        {
            var parts = FilenameBox.Text.Split('.');
            if (parts.Length != 2)
                throw new ArgumentException();
            var reader = parts[1];
            var editor = (string) WordsFormatListBox.SelectedItem + ":" + (string) PartOfSpeechListBox.SelectedItem;
            var builder = (string) BuildAlgorithmListBox.SelectedItem;
            var drawer = (string) ImageFormatListBox.SelectedItem;
            return new ImplementationName(reader, editor, builder, drawer);
        }

        private Settings GetSettings()
        {
            var color = Color.FromName((string) ColorsComboBox.SelectedItem);
            var fontFamily = FontFamily.Families[FontsListBox.SelectedIndex];
            var center = new Point(Width / 2, Height / 2);
            var bitmap = new Bitmap(Width, Height);
            var inputFileName = FilenameBox.Text;
            return new Settings(color, fontFamily, center, bitmap, inputFileName);
        }

        private int Width
        {
            get
            {
                if (int.TryParse(WidthBox.Text, out var n))
                    return n;
                throw new ArgumentException();
            }
        }

        private int Height
        {
            get
            {
                if (int.TryParse(HeightBox.Text, out var n))
                    return n;
                throw new ArgumentException();
            }
        }


        private const int HeightBtwFields = 35;
        private const int HeightBtwLabelAndField = 15;
        private const int WidthBtwFields = 5;

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
            Text = "512"
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
            Text = "512"
        };

        private static Button BuildButton { get; } = new Button
        {
            Text = "Build cloud",
            Font = DefaultFont,
            Location = new Point(ColorsComboBox.Location.X, ColorsComboBox.Location.Y + HeightBtwFields),
            Width = 256,
            Height = 30
        };

        private PictureBox Picture { get; } = new PictureBox
        {
            Width = 512,
            Height = 512,
            Location = new Point(ImageFormatListBox.Location.X + ImageFormatListBox.Width + WidthBtwFields, 0)
        };

        private Dictionary<Type, Action<string, Type>> updaters = new Dictionary<Type, Action<string, Type>>
        {
            [typeof(ITextRectanglesDrawer)] = (s, type) =>
            {
                registring.Add(new RegistringImplemetation(s, type));
                ImageFormatListBox.Items.Add(s);
            },
            [typeof(ITagsCloudBuilder)] = (s, type) =>
            {
                registring.Add(new RegistringImplemetation(s, type));
                BuildAlgorithmListBox.Items.Add(s);
            }
        };

        public void UpdateProgram(string str, Type type)
        {
            if (updaters.ContainsKey(type))
                updaters[type](str, type);
        }

        public void UpdateWordsEditor(Type type, string editor = "No format", string filter = "All")
        {
            registring.Add(new RegistringImplemetation(editor + ":" + filter, type));
            if (editor != "No format")
                WordsFormatListBox.Items.Add(editor);
            if (filter != "All")
                PartOfSpeechListBox.Items.Add(filter);
        }
    }
}