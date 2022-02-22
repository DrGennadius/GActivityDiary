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
        private ActivityType? _activityType;

        public ActivityTypeWindowViewModel(DbContext dbContext, ActivityType? activityType)
        {
            DbContext = dbContext
                ?? throw new ArgumentNullException(nameof(dbContext));

            _activityType = activityType;
            if (_activityType != null)
            {
                Name = _activityType.Name;
                Cost = _activityType.Cost;
            }

            SaveActivityTypeCmd = ReactiveCommand.CreateFromTask(() => SaveActivityTypeAsync());
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

        public ReactiveCommand<Unit, ActivityType?> SaveActivityTypeCmd { get; }

        public ReactiveCommand<Unit, ActivityType?> CancelCmd { get; }

        public ActivityType? Cancel()
        {
            return null;
        }

        public async Task<ActivityType?> SaveActivityTypeAsync()
        {
            if (_activityType == null)
            {
                _activityType = new(Name, Cost);
            }
            else
            {
                _activityType.Name = Name;
                _activityType.Cost = Cost;
            }
            var uid = await DbContext.ActivityTypes.SaveAsync(_activityType);
            _activityType = await DbContext.ActivityTypes.GetByIdAsync(uid);
            return _activityType;
        }
    }
}
