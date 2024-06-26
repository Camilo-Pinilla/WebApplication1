﻿using ChartsProject.Models;
using ChartsProject.Services;
using ChartsProject.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace ChartsProject.Controllers
{
    public class ChartController : Controller
    {
        private readonly ILogger<ChartController> _logger;
        private readonly GithubHttpService _githubService;
        private readonly NpmHttpService _npmService;

        public ChartController(ILogger<ChartController> logger, GithubHttpService githubService, NpmHttpService npmService)
        {
            _logger = logger;
            _githubService = githubService;
            _npmService = npmService;
        }
        public async Task<IActionResult> SimpleChart()
        {

            List<Source> sourceList =
            [
                new() {Owner = "nodejs", Repo = "node"},
                new() {Owner = "oven-sh", Repo = "bun"},
                new(){Owner = "denoland", Repo = "deno"}
            ];
            List<SimpleChart> simpleChartList = [];

            foreach (var source in sourceList)
            {
                var operationResult = await _githubService.RetrieveStargazersCount(source.Owner, source.Repo);
                if (operationResult.IsSuccess)
                {
                    _logger.LogInformation("The {count} stargazers from {owner}/{repo} were retrieved correctly", operationResult.Data, source.Owner, source.Repo);
                    simpleChartList.Add(new SimpleChart { Label = source.Repo, Value = operationResult.Data });
                }
                else
                {
                    _logger.LogError(operationResult.ErrorMessage, operationResult.Exception);
                }
            }

            return Json(simpleChartList);
        }


        public async Task<IActionResult> PlottingChart()
        {
            OperationResult<dynamic> result = await _npmService.RetrieveDownloadRecords("nodejs", "bun", "deno");
            if (result.IsSuccess)
            {
                return Json(result.Data);
            }
            else
            {
                return Content("Something went wrong");
            }
        }
    }
}
