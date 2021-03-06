﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageContest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace ImageContest.Pages
{
    public class IndexModel : PageModel
    {
        public IndexModel(IConfiguration config)
        {
            Config = config;
        }
        [BindProperty]
        public SearchModel SearchDetails {get; set;}
        public IConfiguration Config { get; }

        public void OnGet()
        {
        }

        public IActionResult OnPost() {
            if (!ModelState.IsValid) {
                return Page();
            }
            
            // check for an existing guess
            if (SearchDetails.Guess > 0) {
                if ((SearchDetails.Guess == 1 && SearchDetails.ImgUrl1SearchIndex == 0) ||
                    (SearchDetails.Guess == 2 && SearchDetails.ImgUrl2SearchIndex == 0)) {
                        SearchDetails.Message = $"You guessed correctly! That's the number 1 search result for \"{SearchDetails.SearchTerm}\" on Bing Images";
                } else {
                    var searchIndex = (SearchDetails.Guess == 1 ? SearchDetails.ImgUrl1SearchIndex : SearchDetails.ImgUrl2SearchIndex);
                    SearchDetails.Message = $"Incorrect! This image was search result number {searchIndex} for \"{SearchDetails.SearchTerm}\" on Bing Images";
                }
                return Page();
            }

            // get two images given the search string
            var imageSearcher = new ImageSearch.ImageSearcher(Config["BingImageApiKey"]);
            var results = imageSearcher.GetImageSearchResults(SearchDetails.SearchTerm);
            // randomly choose which will be the first image vs second image
            if (new Random().Next(1) == 1) {
                SearchDetails.ImgUrl1 = results.Img1Url;
                SearchDetails.ImgUrl2 = results.Img2Url;
                SearchDetails.ImgUrl1SearchIndex = 0;
                SearchDetails.ImgUrl2SearchIndex = results.Img2Index;
            } else {
                SearchDetails.ImgUrl1 = results.Img2Url;
                SearchDetails.ImgUrl2 = results.Img1Url;
                SearchDetails.ImgUrl1SearchIndex = results.Img2Index;
                SearchDetails.ImgUrl2SearchIndex = 0;
            }
            return Page();
        }
    }
}
