using System;
using System.IO;
using System.Net;
using HarmonyLib;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using static JoinCodeSender.Plugin;

namespace DiscordMessenger;

[HarmonyPatch]
[Serializable]
public class DiscordMessage
{
    [YamlMember(Alias = "username")] public string Username { get; set; }

    [YamlMember(Alias = "avatar_url", ApplyNamingConventions = false)]
    public string AvatarUrl { get; set; }

    [YamlMember(Alias = "content")] public string Content { get; set; }

    [YamlMember(Alias = "tts")] public bool TTS { get; set; }

    public DiscordMessage SetUsername(string username)
    {
        Username = username;
        return this;
    }

    public DiscordMessage SetAvatar(string avatar)
    {
        AvatarUrl = avatar;
        return this;
    }

    public DiscordMessage SetContent(string content)
    {
        Content = content;
        return this;
    }

    public DiscordMessage SetTTS(bool tts)
    {
        TTS = tts;
        return this;
    }

    public void SendMessage(string url)
    {
        var webClient = new WebClient();
        webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
        var serializer = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
        var yaml = serializer.Serialize(this);
        var r = new StringReader(yaml);
        var deserializer = new DeserializerBuilder().Build();
        var yamlObject = deserializer.Deserialize(r);

        var serializerJson = new SerializerBuilder().JsonCompatible().Build();
        if (yamlObject != null)
        {
            var json = serializerJson.Serialize(yamlObject);
            webClient.UploadString(url, serializerJson.Serialize(json));
        } else
        {
            throw new Exception("Failed to serialize yaml object");
        }
    }


    public void SendMessageAsync(string url)
    {
        var serializer = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
        var yaml = serializer.Serialize(this);
        var r = new StringReader(yaml);
        var deserializer = new DeserializerBuilder().Build();
        var yamlObject = deserializer.Deserialize(r);

        var serializerJson = new SerializerBuilder().JsonCompatible().Build();
        if (yamlObject != null)
        {
            var json = serializerJson.Serialize(yamlObject);
            PostToDiscord(json, url);
        } else
        {
            DebugError("Failed to serialize yaml object", true);
        }
    }

    public static async void PostToDiscord(string content, string url)
    {
        if (content == "" || url == "") return;

        var discordAPI = WebRequest.Create(url);
        discordAPI.Method = "POST";
        discordAPI.ContentType = "application/json";
        discordAPI.ContentLength = content.Length;
        var stream = await discordAPI.GetRequestStreamAsync();
        using (var streamWriter = new StreamWriter(stream)) streamWriter.Write(content);
    }
}