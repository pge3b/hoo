﻿using System.Runtime.CompilerServices;
using Hoo.Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace HooService.Common
{
    public interface IFileProviderService
    {
        Task<IEnumerable<FileItemModel>> GetFilesAsync();

        /*
        Task<IActionResult> GetFile(string sourceId, string filePath);

        Task<IActionResult> GetSource(string sourceId);
        */
    }
}
