﻿using Hoo.Service.Models;
using Hoo.Service.Services.GoogleDrive;
using Hoo.Service.Services.OneDrive;
using Hoo.Service.Services.WebFiles;
using HooService.Common;
using Microsoft.AspNetCore.Mvc;

namespace Hoo.Service.Controllers
{
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileProviderService _fileProviderService;
        private readonly ILogger<FileController> _logger;

        public FileController(ILogger<FileController> logger, IFileProviderService fileProviderService)
        {
            _logger = logger;
            _fileProviderService = fileProviderService;
        }

        [HttpGet]
        [Route("GetFiles")]
        public async Task<FileItemModel[]> GetFiles()
        {
            return (await _fileProviderService.GetFilesAsync()).ToArray();
        }
    }
}
