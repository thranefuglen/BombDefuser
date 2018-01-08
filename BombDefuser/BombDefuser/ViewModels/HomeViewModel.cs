using BombDefuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BombDefuser.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
		//Event - View er subscribed.
	    public event EventHandler OnBombexploded;
	    public void ExplodeBombEvent()
	    {
		    EventHandler handler = OnBombexploded;
		    if (handler != null) //Sørger for at der er subscribers. Hvis ikke, kører den ikke.
		    {
			    handler(this, EventArgs.Empty); //This = HVM  |  Ingen args.
		    }
		}

		//Bomb Game
		private PointScore _pointScore;
	    string bomb = new Random().Next(1, 4).ToString();
		private string _totalBombsMessage;
	    private string _currentBombsDefusedMessage;
	    public TaskModel TaskModel { get; set; }

	    public PointScore PointScore
	    {
		    get { return _pointScore; }
		    set
		    {
			    _pointScore = value;
			    OnPropertyChanged();
		    }
	    }

	    public HomeViewModel()
        {
			PointScore = new PointScore
			{
				totalScore = 0,
				currentScore = 0
			};
			TaskModel = new TaskModel
			{
				title = "Din titel",
				duration = 2
			};

        }

	    public Command PressBombCommand //Command Linked til alle Buttons/bombs. Checker bombe og kalder UpdateCommands som opdatere værdier og derefter message (Label).
	    {
		    get
		    {
				return new Command<string>((buttonNr) =>
					{
						if (buttonNr == bomb) //Explode
						{
							bomb = new Random().Next(1, 4).ToString();
							PointScore.currentScore = -1;
							UpdateCurrentBombsCommand.Execute(null); //Kalder her update, for at OnPropertyChanged() bliver kaldt. Ellers opdateres Current ikke til 0 før næste defuse.
							ExplodeBombEvent();
						}
						else //Defuse
						{
							UpdateCurrentBombsCommand.Execute(null);
							UpdateTotalBombsCommand.Execute(null);
							bomb = new Random().Next(1, 4).ToString();
						}
					});
		    }
	    }

	    public Command UpdateTotalBombsCommand //Command til opdatering af Totalscore værdi. Opdatere Message. Message er bundet til View via Binding
	    {
		    get
		    {
			    return new Command(() => {
				    PointScore.totalScore += 1;
					TotalBombsMessage = "Total: " + PointScore.totalScore;
			    });
			}
	    }

	    public string TotalBombsMessage
	    {
		    get { return _totalBombsMessage; }
		    set
		    {
			    _totalBombsMessage = value;
				OnPropertyChanged();
		    }
	    }

	    public Command UpdateCurrentBombsCommand //Command til opdatering af current score værdi. Opdatere Message. Message er bundet til View via Binding
		{
		    get
		    {
				return new Command(() => {
					PointScore.currentScore += 1;
					CurrentBombsDefusedMessage = "Current: " + PointScore.currentScore;
				});
		    }
	    }

	    public string CurrentBombsDefusedMessage
		{
		    get { return _currentBombsDefusedMessage; }
		    set
		    {
			    _currentBombsDefusedMessage = value;
				OnPropertyChanged();
		    }
	    }


	    public event PropertyChangedEventHandler PropertyChanged;  //Interface til INotifyPropertyChanged

	    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	    {
		    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	    }

	    
    }
}
