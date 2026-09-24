using Microsoft.EntityFrameworkCore;

namespace Cicekci.Data;

// Eski sürüm EnsureCreated ile kurulmuş veri tabanlarındaki SiteContents tablosunu
// yeni düzenlenebilir alanlara yükseltir. Yalnızca kolon ekler, veri oluşturmaz.
public static class LegacySiteContentSchema
{
    public static void Upgrade(ApplicationDbContext db)
    {
        if (db.Database.ProviderName != "Microsoft.EntityFrameworkCore.SqlServer") return;

        var columns = new (string Name, int Length)[]
        {
            ("HomeHeroImageUrl", 2048), ("FeaturedTitle", 150), ("WhyTitle", 150),
            ("Benefit1Title", 100), ("Benefit1Description", 300),
            ("Benefit2Title", 100), ("Benefit2Description", 300),
            ("Benefit3Title", 100), ("Benefit3Description", 300),
            ("Benefit4Title", 100), ("Benefit4Description", 300),
            ("AboutImageUrl", 2048), ("ContactTitle", 150)
        };

        foreach (var (name, length) in columns)
        {
            // name/length sadece bu dosyadaki sabitlerden gelir, kullanıcı girdisi değildir.
            #pragma warning disable EF1002 // Kolon adları ve uzunluklar sabit beyaz listedendir.
            db.Database.ExecuteSqlRaw($"""
                IF OBJECT_ID(N'dbo.SiteContents', N'U') IS NOT NULL
                   AND COL_LENGTH(N'dbo.SiteContents', N'{name}') IS NULL
                    ALTER TABLE [dbo].[SiteContents] ADD [{name}] nvarchar({length}) NULL;
                """);
            #pragma warning restore EF1002
        }
    }
}
