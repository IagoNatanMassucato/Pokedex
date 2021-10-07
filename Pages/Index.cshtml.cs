using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Pokedex.Classes;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace Pokedex.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public List<Pokemon> ListaPokemon { get; set; }
        
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(string Tipo)
        {
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("Pokemons")))
            {
                PopularLista();
            }
            var pokemons = JsonSerializer.Deserialize<List<Pokemon>>(HttpContext.Session.GetString("Pokemons"));
            var tipos = pokemons.SelectMany(pokemons => pokemons.Tipo).Distinct().ToList();
            tipos.Sort();
            ViewData["Tipos"] = tipos;
            ListaPokemon = string.IsNullOrEmpty(Tipo) ? pokemons : pokemons.Where(p => p.Tipo.Contains(Tipo)).ToList();
        }    

        public void PopularLista()
        {
            using (StreamReader leitor = new StreamReader(@"Dados\dados.json"))
            {
                string dados = leitor.ReadToEnd();
                HttpContext.Session.SetString("Pokemons", dados);
            }
        }
            
    }
}
