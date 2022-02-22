using GActivityDiary.Core.DataBase;
using GActivityDiary.Core.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace GActivityDiary.GUI.Avalonia.ViewModels
{
    public class ActivityTypeWindowViewModel : ViewModelBase
    {
        private string _name = "";
        private decimal _cost = 0;

        public ActivityTypeWindowViewModel(DbContext dbContext, ActivityTypeTableWindowViewModel activityTypeTableWindowViewModel)
        {
            DbContext = dbContext
                ?? throw new ArgumentNullException(nameof(dbContext));
            ActivityTypeTableWindowViewModel = activityTypeTableWindowViewModel
                ?? throw new ArgumentNullException(nameof(activityTypeTableWindowViewModel));

            SaveActivityTypeCmd = ReactiveCommand.Create(() => SaveActivityType());
            CancelCmd = ReactiveCommand.Create(() => Cancel());
        }

        [Required]
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        public decimal Cost 
        { 
            get => _cost; 
            set => this.RaiseAndSetIfChanged(ref _cost, value);
        }

        /// <summary>
        /// Database context.
        /// </summary>
        public DbContext DbContext { get; private set; }

        public ActivityTypeTableWindowViewModel ActivityTypeTableWindowViewModel { get; }

        public ReactiveCommand<Unit, ActivityType?> SaveActivityTypeCmd { get; }

        public ReactiveCommand<Unit, ActivityType?> CancelCmd { get; }

        public ActivityType? Cancel()
        {
            return null;
        }

        public ActivityType? SaveActivityType()
        {
            ActivityType activityType = new(Name, Cost);
            var uid = DbContext.ActivityTypes.Save(activityType);
            activityType = DbContext.ActivityTypes.GetById(uid);
            return activityType;
        }
    }
}
