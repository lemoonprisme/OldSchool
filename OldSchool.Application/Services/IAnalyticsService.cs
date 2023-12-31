﻿using OldSchool.Application.Models;

namespace OldSchool.Application.Services;

public interface IAnalyticsService
{
    public Task<SchoolAnalytics> GetStatistics();
}