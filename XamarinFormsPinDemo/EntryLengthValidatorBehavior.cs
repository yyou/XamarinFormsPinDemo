using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinFormsPinDemo
{
    // source code from https://forums.xamarin.com/discussion/19285/max-length-on-entry
    // all credit to @Atalia and @Mugs
    public class EntryLengthValidatorBehavior : Behavior<Entry> {
        public int MaxLength { get; set; }

        protected override void OnAttachedTo(Entry bindable) {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnEntryTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable) {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnEntryTextChanged;
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs e) {
            var entry = (Entry)sender;

            // if Entry text is longer then valid length
            if (entry.Text.Length > this.MaxLength) {
                string entryText = entry.Text;
                entry.TextChanged -= OnEntryTextChanged;
                entry.Text = e.OldTextValue;
                entry.TextChanged += OnEntryTextChanged;
            }
        }
    }
}
