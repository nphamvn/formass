using FormaaS;
using FormaaS.Entities;
using FormaaS.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    .EnableSensitiveDataLogging()
    .LogTo(Console.WriteLine)
    ;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("forms", async (AppDbContext dbContext, FormCreateUpdateRequest formCreateUpdateRequest) =>
{
    using (var trans = await dbContext.Database.BeginTransactionAsync())
    {
        try
        {
            //Insert form
            var form = new Form
            {
                Name = formCreateUpdateRequest.Name,
                CreatedAt = DateTime.UtcNow
            };

            await dbContext.Forms.AddAsync(form);
            await dbContext.SaveChangesAsync();

            //Insert form fields
            var formFiels = formCreateUpdateRequest.Fields.Select(f => new FormField
            {
                Id = f.Id,
                //Name = f.Name,
                //Type = f.Type,
            });

            await dbContext.FormFields.AddRangeAsync();

            await trans.CommitAsync();
        }
        catch (Exception)
        {
            await trans.RollbackAsync();
            throw;
        }
    }

    return Results.Created();
});

app.MapPut("forms/{id:int}", async (AppDbContext dbContext, int id, FormCreateUpdateRequest formCreateUpdateRequest) =>
{
    using (var trans = await dbContext.Database.BeginTransactionAsync())
    {
        try
        {
            var form = await dbContext.Forms.Include(x => x.Fields).SingleAsync(x => x.Id == id);

            form.Name = formCreateUpdateRequest.Name;
            form.UpdatedAt = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();

            var allFieldIds = formCreateUpdateRequest.Fields.Select(f => f.Id).ToList();
            var updateFields = await dbContext.FormFields.Where(x => allFieldIds.Contains(x.Id)).ToListAsync();
            foreach (var field in updateFields)
            {
                var postField = formCreateUpdateRequest.Fields.Single(f => f.Id == field.Id);

                field.Name = postField.Name;
                field.CreatedAt = DateTime.UtcNow;
                field.UpdatedAt = DateTime.UtcNow;

                switch (postField.Details)
                {
                    case FormFieldTextInputDetails textInputDetails:
                        field.Details = new()
                        {
                            VirtualDictionary = new Dictionary<string, object>()
                            {
                                { nameof(textInputDetails.MinLength), textInputDetails.MinLength },
                                { nameof(textInputDetails.MaxLength), textInputDetails.MaxLength },
                            }
                        };
                        break;
                    default:
                        break;
                }
            }

            var addFieldIds = allFieldIds.Except(updateFields.Select(f => f.Id));
            var addFields = formCreateUpdateRequest.Fields.Where(x => addFieldIds.Contains(x.Id)).Select(postField =>
            {
                var field = new FormField
                {
                    Id = postField.Id,
                    Form = form,
                    Name = postField.Name,
                    Type = postField.Type,
                };

                switch (postField.Details)
                {
                    case FormFieldTextInputDetails textInputDetails:
                        field.Details = new()
                        {
                            VirtualDictionary = new Dictionary<string, object>()
                            {
                                { nameof(textInputDetails.MinLength), textInputDetails.MinLength },
                                { nameof(textInputDetails.MaxLength), textInputDetails.MaxLength },
                            }
                        };
                        break;
                    default:
                        break;
                }

                return field;
            });

            await dbContext.FormFields.AddRangeAsync(addFields);

            await dbContext.SaveChangesAsync();

            await trans.CommitAsync();
        }
        catch (Exception)
        {
            await trans.RollbackAsync();
            throw;
        }
    }

    return Results.NoContent();
});

app.MapGet("forms", async (AppDbContext dbContext) =>
{
    var forms = await dbContext.Forms.Include(x => x.Fields).AsNoTracking().ToListAsync();
    return Results.Ok(forms.Select(f => new
    {
        f.Id,
        f.Name,
        Fields = f.Fields.Select(f => new
        {
            f.Id,
            f.Name,
            f.Type,
            Details = f.Details.VirtualDictionary
        })
    }));
});

app.MapGet("forms/{id:int}", async (AppDbContext dbContext, int id) =>
{
    var form = await dbContext.Forms.Include(x => x.Fields).SingleAsync(x => x.Id == id);
    return Results.Ok(form);
});

app.Run();
