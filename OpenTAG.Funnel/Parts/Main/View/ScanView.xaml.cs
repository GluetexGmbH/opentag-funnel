using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTAG.Funnel.Parts.Main.ViewModel;

namespace OpenTAG.Funnel.Parts.Main.View;

public partial class ScanView : ContentPage
{
    private readonly ScanViewModel viewModel;

    public ScanView(ScanViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        this.viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BuildFieldsUi();
    }

    private void BuildFieldsUi()
    {
        FieldsContainer.Clear();

        if (viewModel.LoadedTemplate is null)
            return;

        if (viewModel.LoadedTemplate?.Fields is null)
            return;

        foreach (var field in viewModel.LoadedTemplate.Fields)
        {
            if (field.Type != "NUMBER" && field.Type != "TEXT")
                continue;

            // Create field container
            var fieldLayout = new VerticalStackLayout
            {
                Margin = new Thickness(0, 0, 0, 15)
            };

            // Add label and description
            fieldLayout.Add(new Label
            {
                Text = field.DisplayName,
                FontAttributes = FontAttributes.Bold,
                FontSize = 18
            });

            string description = field.Description;
            if (string.IsNullOrEmpty(description))
                description = "No description available.";

            fieldLayout.Add(new Label
            {
                Text = description,
                FontSize = 14,
                TextColor = Colors.Gray,
                Margin = new Thickness(0, 5, 0, 10)
            });

            switch (field.Type)
            {
                case "TEXT":
                {
                    var textEntry = new Entry
                    {
                        Text = field.Value
                    };
                    textEntry.TextChanged += (s, e) => field.Value = textEntry.Text;
                    fieldLayout.Add(textEntry);
                    break;
                }
                case "NUMBER":
                {
                    var numericEntry = new Entry
                    {
                        Text = field.Value,
                        Keyboard = Keyboard.Numeric
                    };
                    numericEntry.TextChanged += (s, e) => {
                        if (!int.TryParse(e.NewTextValue, out int _))
                        {
                            numericEntry.Text = new string(e.NewTextValue.Where(char.IsDigit).ToArray());
                        }
                        else
                        {
                            field.Value = numericEntry.Text;
                        }
                    };
                    fieldLayout.Add(numericEntry);
                    break;
                }
            }

            FieldsContainer.Add(fieldLayout);
        }
    }
}