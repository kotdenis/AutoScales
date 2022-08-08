using AutoScales.Data;
using AutoScales.Data.Entities;
using AutoScales.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace AutoScales.Server
{
    public static class SeedData
    {
        public static async Task PopulateDb(WebApplicationBuilder application)
        {
            var sp = application.Services.BuildServiceProvider();
            using var temp = sp.CreateScope();
            var scopedServices = temp.ServiceProvider;

            var context = scopedServices.GetRequiredService<ScalesDbContext>();
            if (!context.Set<Transport>().Any())
            {
                await context.AddRangeAsync(new Transport[]
                {
                new Transport { AxisNumber = 3, Cargo = "Electronics", Name = "КамАЗ-65207", Number = "A256AK", Weight = 25450},
                new Transport { AxisNumber = 4, Cargo = "Appliances", Name = "Hyundai Mighty", Number = "H698KO", Weight = 15890},
                new Transport { AxisNumber = 2, Cargo = "Cosmetic", Name = "ISUZU ELF", Number = "O745HB", Weight = 9250},
                new Transport { AxisNumber = 6, Cargo = "Fertilizers", Name = "МАЗ-6310", Number = "A963KO", Weight = 40650},
                new Transport { AxisNumber = 2, Cargo = "Cosmetic", Name = "ГАЗ «Садко»", Number = "E123BH", Weight = 5140},
                new Transport { AxisNumber = 4, Cargo = "Furniture", Name = "HOWO A7", Number = "T574TB", Weight = 22360},
                new Transport { AxisNumber = 2, Cargo = "Furniture", Name = "JAC N-56", Number = "P897HC", Weight = 4890},
                new Transport { AxisNumber = 5, Cargo = "Fertilizers", Name = "МАЗ-5440", Number = "K321KK", Weight = 45740},
                new Transport {AxisNumber = 3, Cargo = "Logs", Name = "MAN TGS", Number = "H477CC", Weight = 12780},
                new Transport {AxisNumber = 3, Cargo = "Logs", Name = "Scania «G-Series»", Number = "P521OK", Weight = 13410},
                new Transport { AxisNumber = 3, Cargo = "Wood boards", Name = "КрАЗ М16.1Х", Number = "H147KA", Weight = 8170},
                new Transport { AxisNumber = 3, Cargo = "Logs", Name = "Volvo серии «FH»", Number = "O686PO", Weight = 15320},
                new Transport {AxisNumber = 5, Cargo = "Wheat", Name ="ISUZU GIGA 6х4", Number = "C114HA", Weight = 51240},
                new Transport { AxisNumber = 6, Cargo = "Seed", Name = "КрАЗ-6230C40", Number = "K931CA", Weight = 58120},
                new Transport {AxisNumber = 3, Cargo = "Wheat", Name = "КамАЗ-689011", Number = "M665MH", Weight = 26510},
                new Transport {AxisNumber = 2, Cargo = "Coal", Name = "БелАЗ-75320", Number = "B127KB", Weight = 31240},
                new Transport {AxisNumber = 6, Cargo = "Fertilizers", Name = "Тонар-7502", Number = "T788TT", Weight = 74250},
                new Transport {AxisNumber = 3, Cargo = "Crushed stone", Name = "MAN TGS", Number = "A441KM", Weight = 26100},
                new Transport {AxisNumber = 3, Cargo = "Coal", Name = "Тонар-7501", Number = "P333OP", Weight = 53280},
                new Transport {AxisNumber = 3, Cargo = "Crushed stone", Name = "Тонар-45251", Number = "M149KC", Weight = 37800}
                });
                await context.SaveChangesAsync(); 

                var userManager = scopedServices.GetRequiredService<UserManager<User>>();
                var user = new User
                {
                    Email = "admin@test.com",
                    FirstName = "Den",
                    LastName = "Kot",
                    UserName = "Den"
                };
                await userManager.CreateAsync(user, "Secret123$");
                var roleManager = scopedServices.GetRequiredService<RoleManager<Role>>();
                await roleManager.CreateAsync(new Role { Name = RoleConstants.Admin });
                var admin = await userManager.FindByEmailAsync(user.Email);
                await userManager.AddToRoleAsync(admin, RoleConstants.Admin);
            }
            if (!context.Set<TransportQuantity>().Any())
            {
                await context.AddAsync(new TransportQuantity { Quantity = 20 });
                await context.SaveChangesAsync();
            }
        }
    }
}
