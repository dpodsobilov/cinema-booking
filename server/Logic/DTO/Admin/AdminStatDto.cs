﻿namespace Logic.DTO.Admin;

public class AdminStatDto
{
    public string FilmName { get; set; } = null!;

    public int OrderedTickets { get; set; }

    public int TotalTickets { get; set; }
    
    public int Percentage { get; set; }
}