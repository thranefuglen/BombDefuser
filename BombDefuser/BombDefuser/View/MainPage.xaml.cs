using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using BombDefuser.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using BombDefuser.ViewModels;

namespace BombDefuser
{
    public partial class MainPage : ContentPage
    {
        static string bomb = new Random().Next(1, 4).ToString();
        static int scores = 0;
		HomeViewModel HVM;
        public MainPage()
        {
			HVM = new HomeViewModel();
			BindingContext = HVM;
	        
			InitializeComponent();

	        //Subscribe dette View til vores HVM
	        HVM.OnBombexploded += HandleBombExplosion;

			//Sætte Source til bombe img  |  I xaml hedder img: <Image x:Name="bombImg"/>
			bombImg.Source = "icon.png";
	        bombImg.Opacity = 0;
	        bombImg.TranslationY -= 80;
	        bombImg.TranslationX += 50;
        }

		//Når Subscribed Event affyres i HVM, Kører denne handle.
	    public async void HandleBombExplosion(object sender, EventArgs args)
	    {
		    Debug.WriteLine("Bomb Exploded!");

			//Animation
		    bombImg.Opacity = 100;
			 //Når alle animationer er færdige, køres "reset" af bombe animation.
		    await bombImg.ScaleTo(7, 450); //Denne har ingen await, så metode kald fortsætter. Await under stopper videre kald. Sørg for at tid i denne går op med await total tid.
		    await bombImg.RotateXTo(25, 50);
		    await bombImg.RotateYTo(25, 50);

			//Reset bombe animation
			bombImg.Rotation = 0;
		    bombImg.Scale = 1;
		    bombImg.RotationX = 0;
		    bombImg.RotationY = 0;
		    bombImg.Opacity = 0;
	    }

		async void ButtonClicked(object sender, EventArgs e)//sender = button that is clicked, e contains data from sender object
		{
			Button button = sender as Button;

			//Game over
			if (button.Text == bomb)
			{
				await DisplayAlert("Bomb Exploded", "GAME OVER - You Defused " + scores + " Bombs!", "Retry");
				bomb = new Random().Next(1, 4).ToString();
				scores = 0;
			}
			else //Didn't explode the bomb
			{
				scores += 1;
				await DisplayAlert("Bomb Defused!", "Scores: " + scores, "Continue");
				bomb = new Random().Next(1, 4).ToString();
			}
		}

	}
}
