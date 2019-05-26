# XamarinFormsPinDemo

This is a demo program for PIN entry in Xamarin Forms platform. This program is inspired by Alex Dunn's implementation at https://alexdunn.org/2017/03/30/xamarin-controls-xamarin-forms-pinview/ .

In Alex's solution, the PIN is shown in 4 different small Entries. The first Entry box gets focus initially when the page is loaded. With help of Entry control's TextChanged event and a custom behavior class, when user enters a digit, control focus can move to next Entry.

In the solution of this repository, the PIN are actually entered into a hidden Entry and the 4 visible Entries are just for displaying the digits. Therefore, the visible Entries can be replaced with any other controls.
