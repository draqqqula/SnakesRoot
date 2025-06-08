using MatchMakingLogic.Party.Implementation.Generators;
using MatchMakingLogic.Party.Interfaces;
using MatchMakingLogic.Party.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMakingLogic.Party.Implementation;

internal class PartyManager(IConfiguration configuration) : IPartyManager
{
    private readonly Dictionary<PartyCode, Party> _parties = [];
    private readonly ICodeGenerator _generator = new WordGenerator(configuration);

    public PartyCode CreateFor(Client owner)
    {
        var code = _generator.New();
        var party = new Party(owner);
        _parties.Add(code, party);
        party.OnDismiss += () => { _parties.Remove(code); _generator.Free(code); };
        return code;
    }

    public IPartyController? TryGetByCode(PartyCode code)
    {
        if (_parties.TryGetValue(code, out var controller))
        {
            return controller;
        }
        return default;
    }
}
