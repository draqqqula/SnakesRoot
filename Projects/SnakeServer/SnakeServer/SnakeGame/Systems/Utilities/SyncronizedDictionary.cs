using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using ServerEngine.Interfaces;
using ServerEngine.Interfaces.Services;
using ServerEngine.Models;
using SnakeGame.Systems.RuntimeCommands;
using SnakeGame.Systems.RuntimeCommands.Interfaces;
using SnakeGame.Systems.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Utilities;

internal class SyncronizedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ISessionService
{
    private readonly IRuntimeCommandFactory _runtimeCommandFactory;
    private readonly IClientRegistry _clientRegistry;
    private RuntimeCommand<TKey, TValue> _updateKvpCommand;
    private RuntimeCommand<TKey> _removeKeyCommand;

    public SyncronizedDictionary(IServiceProvider provider, string name)
    {
        _runtimeCommandFactory = provider.GetRequiredService<IRuntimeCommandFactory>();
        _clientRegistry = provider.GetRequiredService<IClientRegistry>();

        _updateKvpCommand =
        new RuntimeCommand<TKey, TValue>(name + "_update", _runtimeCommandFactory);

        _removeKeyCommand =
        new RuntimeCommand<TKey>(name + "_remove", _runtimeCommandFactory);
    }

    private IDictionary<TKey, TValue> _local = new Dictionary<TKey, TValue>();


    private void UpdateKvp(TKey key, TValue value)
    {
        foreach (var client in _clientRegistry.Online)
        {
            _updateKvpCommand.Send(client, key, value);
        }
    }

    public TValue this[TKey key]
    {
        get
        {
            return _local[key];
        }
        set
        {
            UpdateKvp(key, value);
            _local[key] = value;
        }
    }

    public ICollection<TKey> Keys => _local.Keys;

    public ICollection<TValue> Values => _local.Values;

    public int Count => _local.Count;

    public bool IsReadOnly => false;

    public void Add(TKey key, TValue value)
    {
        _local.Add(key, value);
        UpdateKvp(key, value);
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        _local.Add(item);
        UpdateKvp(item.Key, item.Value);
    }

    public void Clear()
    {
        foreach (var client in _clientRegistry.Online)
        {
            foreach (var key in _local.Keys)
            {
                _removeKeyCommand.Send(client, key);
            }
        }
        _local.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return _local.Contains(item);
    }

    public bool ContainsKey(TKey key)
    {
        return _local.ContainsKey(key);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        _local.CopyTo(array, arrayIndex);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return _local.GetEnumerator();
    }

    public bool Remove(TKey key)
    {
        if (_local.Remove(key))
        {
            foreach (var client in _clientRegistry.Online)
            {
                _removeKeyCommand.Send(client, key);
            }
            return true;
        }
        return false;
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        if (_local.Remove(item))
        {
            foreach (var client in _clientRegistry.Online)
            {
                _removeKeyCommand.Send(client, item.Key);
            }
            return true;
        }
        return false;
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        return _local.TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _local.GetEnumerator();
    }

    public void OnJoin(IGameContext context, ClientIdentifier id)
    {
        foreach (var kvp in _local)
        {
            _updateKvpCommand.Send(id, kvp.Key, kvp.Value);
        }
    }

    public void OnLeave(IGameContext context, ClientIdentifier id)
    {
    }
}

public static class SyncronizedDictionaryInstaller
{
    public static void AddSyncronizedDictionary<TKey, TValue>(this IServiceCollection services, string name)
    {
        services.AddKeyedSingleton(name, (provider, key) => new SyncronizedDictionary<TKey, TValue>(provider, name));
        services.AddKeyedSingleton<IDictionary<TKey, TValue>>(name, (provider, key) => provider.GetRequiredKeyedService<SyncronizedDictionary<TKey, TValue>>(name));
        services.AddSingleton<ISessionService>(provider => provider.GetRequiredKeyedService<SyncronizedDictionary<TKey, TValue>>(name));
    }
}