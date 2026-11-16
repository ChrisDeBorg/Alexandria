using System;
using System.Collections.Generic;
using System.Text;

namespace Alexandria.Domain.Sources;

class Source
{
    Guid Id;
    Guid ResearchId;

    string Title;
    string Type; // PDF, URL, Text
    string Content;

    ICollection<Chunk> Chunks; // für Vektorisierung
}
