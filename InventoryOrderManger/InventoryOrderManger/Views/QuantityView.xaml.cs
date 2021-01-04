using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InventoryOrderManger.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuantityView : ContentView
    {
        public static readonly BindableProperty QuantityTextProperty = BindableProperty.Create("QuantityText", typeof(string), typeof(QuantityView), string.Empty, BindingMode.TwoWay, null);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(QuantityView), null, BindingMode.TwoWay, null);
        public event EventHandler ValueChanged;
        public QuantityView()
        {
            InitializeComponent();
        }

        public string QuantityText
        {
            get => (string)GetValue(QuantityTextProperty);
            set => SetValue(QuantityTextProperty, value);// add validation
        }

        public object CommandParameter
        {
            get => (object)GetValue(CommandParameterProperty);
            set => SetValue(QuantityTextProperty, value);
        }


        private void Minus_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(QuantityText))
            {
                QuantityText = "0";
            }
            else if (Convert.ToDecimal(QuantityText) > 0)
            {
                QuantityText = (Convert.ToDecimal(QuantityText) - 1).ToString();
            }

            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        private void Plus_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(QuantityText))
            {
                QuantityText = "1";
            }
            else
            {
                QuantityText = (Convert.ToDecimal(QuantityText) + 1).ToString();
            }

            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        private void TextEntry_Completed(object sender, EventArgs e)
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        private void TextEntry_Unfocused(object sender, FocusEventArgs e)
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}