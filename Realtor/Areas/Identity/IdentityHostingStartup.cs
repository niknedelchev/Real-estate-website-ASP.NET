using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Realtor.Data;

[assembly: HostingStartup(typeof(Realtor.Areas.Identity.IdentityHostingStartup))]
namespace Realtor.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
          //      services.AddDefaultIdentity<>(options => options.SignIn.RequireConfirmedAccount = false);
 
            });
        }
    }
}