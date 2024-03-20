﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kreta.Desktop.ViewModels.Base;
using Kreta.HttpService.Services;
using Kreta.Shared.Models;
using Kreta.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Kreta.Desktop.ViewModels.Administration
{
    public partial class EducationLevelViewModel : BaseViewModel
    {
        private readonly IEducationLevelService? _educationLevelService;
        private readonly IStudentService? _studentService;

        [ObservableProperty]
        private ObservableCollection<EducationLevel> _educationLevels = new();

        [ObservableProperty]
        private EducationLevel _selectedEducationLevel = new();

        public EducationLevelViewModel()
        {            
        }
        public EducationLevelViewModel(
            IEducationLevelService? educationLevelService,
            IStudentService? studentService)
        {
            _educationLevelService = educationLevelService;
            _studentService = studentService;
        }

        public string Title { get; set; } = "Tanulmányi szint kezelése";

        public override async Task InitializeAsync()
        {
            await UpdateView();
            await base.InitializeAsync();
        }

        [RelayCommand]
        private void DoNew()
        {
            SelectedEducationLevel = new();
        }

        [RelayCommand]
        private async Task DoRemove(EducationLevel educationLevelToDelete)
        {
            if (_educationLevelService is not null)
            {
                ControllerResponse result= await _educationLevelService.DeleteAsync(educationLevelToDelete.Id);
                if ( result.IsSuccess)
                {
                    await UpdateView();
                }
            }
        }

        [RelayCommand]
        private async Task DoSave(EducationLevel educationLevelToSave)
        {
            if (_educationLevelService is not null)
            {
                ControllerResponse result = new();
                if (educationLevelToSave.HasId)
                    result = await _educationLevelService.UpdateAsync(educationLevelToSave);
                else
                    result = await _educationLevelService.InsertAsync(educationLevelToSave);
                if (result.IsSuccess)
                    await UpdateView();
            }
        }

        [RelayCommand]
        private async Task GetStudentsByEducationLevelId()
        {
            if (_studentService is not null &&
                SelectedEducationLevel is not null &&
                SelectedEducationLevel.HasId)
            {

            }
        }

        private async Task UpdateView()
        {
            if (_educationLevelService is not null)
            {
                List<EducationLevel> result= await _educationLevelService.SelectAllAsync();
                EducationLevels = new ObservableCollection<EducationLevel>(result);
            }
        }
    }
}
