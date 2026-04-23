public async Task<IActionResult> RelatorioCategoriaPais(string categoria, string pais, string skill)
{
    ViewBag.Categorias = await _context.Categoriatalentos
        .Select(c => c.Nome)
        .Distinct()
        .ToListAsync();

    ViewBag.Paises = await _context.Talentos
        .Select(t => t.Pais)
        .Distinct()
        .ToListAsync();

    ViewBag.Skills = await _context.Skills
        .Select(s => s.Nome)
        .Distinct()
        .ToListAsync();

    var resultado = await _relatorioService.GetRelatorioCategoriaPais(categoria, pais, skill);

    return View(resultado);
}