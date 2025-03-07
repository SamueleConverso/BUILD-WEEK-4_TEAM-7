# BUILD-WEEK-4_TEAM-7

## 🛍 Descrizione  

Questa applicazione è un progetto di *e-commerce* sviluppato con *ASP.NET Core Razor Pages*.  
Gli utenti possono *cercare prodotti, **aggiungerli al carrello* e *procedere al checkout, mentre gli **amministratori* possono gestire prodotti e categorie tramite un'interfaccia dedicata.  

---

## ✨ Funzionalità  

✔ *🔎 Ricerca Prodotti* – Trova facilmente i prodotti per nome o categoria.  
✔ *🛒 Carrello* – Aggiungi prodotti, modifica quantità e rimuovi articoli.  
✔ *💳 Checkout* – Riepilogo del carrello e procedura di pagamento.  
✔ *📦 Gestione Prodotti* – Aggiungi, modifica e rimuovi prodotti (solo admin).  
✔ *🗂 Gestione Categorie* – Aggiungi e rimuovi categorie (solo admin).  

---

## 📁 Struttura del Progetto  

📂 *Controllers* (Gestisce le richieste HTTP)  
- 🧐 SearchController.cs → Gestisce la ricerca prodotti  
- 🛒 CartController.cs → Gestisce il carrello  
- 🔧 AdminController.cs → Gestisce l'amministrazione  

📂 *Models* (Definisce la struttura dei dati)  
- 🔎 SearchModel.cs → Modello per la ricerca  
- 📦 Product.cs → Modello per i prodotti  
- 🏷 Category.cs → Modello per le categorie  
- 🛍 Cart.cs → Modello per il carrello  

📂 *Views* (Gestisce l'interfaccia utente)  
- 🛒 Cart/Checkout.cshtml → Vista per il checkout  
- ❌ Shared/Error.cshtml → Vista per la gestione errori  

📂 *wwwroot* (File statici: immagini, script, CSS)  

---

## ⚙ Configurazione  

1️⃣ *Clonare il repository* 🖥

```bash
git clone <repository-url>
```

2️⃣ Configurare la stringa di connessione 🔧
Modificare il file appsettings.json con i dettagli del database:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=NomeServer;Database=BUILDWEEK_TEAM7;User Id=Id;Password=Password;TrustServerCertificate=true;"
  }
}
```

3️⃣ Eseguire le migrazioni del database 📂

```bash
dotnet ef database update
```

4️⃣ Eseguire l'applicazione 🚀

```bash
dotnet run
```

📌 Requisiti

✅ .NET 8.0

✅ SQL Server

🤝 Contributi

I contributi sono benvenuti! 💡
Apri una issue o una pull request per migliorare l'applicazione.

📜 Developers:

Samuele Converso
Tommaso Di Berto Mancini
Daniele Renna
Manuela Lissia
