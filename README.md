# Mini E-Ticaret API

Bu proje, Mini E-Ticaret uygulaması için geliştirilen ASP.NET Core 6 tabanlı bir RESTful API'dir.  
Ürün, kategori, kullanıcı ve sipariş yönetimi gibi temel e-ticaret işlemleri sağlamaktadır.  
Ayrıca Azure Blob Storage entegrasyonu ile resim yükleme desteği bulunmaktadır.

---

## Özellikler

- Ürün yönetimi (CRUD)
- Kategori yönetimi
- Kullanıcı ve rol yönetimi (JWT Authentication)
- Sepet ve sipariş işlemleri
- Azure Blob Storage entegrasyonu (resim yükleme)

---

## Kullanılan Teknolojiler

- ASP.NET Core 6
- Entity Framework Core
- PostgreSQL
- Azure Blob Storage
- JWT Authentication
- AutoMapper
- FluentValidation

---

## Proje Yapısı

- API Katmanı: ASP.NET Core Web API
- Frontend Katmanı: Angular (ayrı bir repoda tutulmaktadır)

---

## Kurulum ve Çalıştırma Adımları

### Gereksinimler

- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- PostgreSQL
- Visual Studio 2022 veya Visual Studio Code

---

### 1️⃣ Projeyi Klonlayın

```bash
git clone https://github.com/FerhatCaliskan/E-Ticaret-API.git
cd E-Ticaret-API
```

### 2️⃣ appsettings.json Yapılandırması

Projeyi çalıştırmadan önce kendi `appsettings.json` dosyanızı oluşturmanız gerekmektedir.  
Aşağıda örnek bir yapı paylaşılmıştır. Gerçek bağlantı bilgilerinizi buradaki alanlara girmeniz gerekir.

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "PostgreSQL": "User ID=your_username;Password=your_password;Host=your_host;Port=5432;Database=your_database_name;"
  },
  "Storage": {
    "Azure": "DefaultEndpointsProtocol=https;AccountName=your_account_name;AccountKey=your_account_key;EndpointSuffix=core.windows.net"
  },
  "BaseStorageUrl": "https://your_account_name.blob.core.windows.net"
}

```
### 3️⃣ Veritabanı Migrasyonları

```bash
dotnet ef database update
```

### 4️⃣ Projeyi Çalıştırma

```bash
dotnet run
```
Projeyi swagger üzerinden çalıştırabilirsiniz.
