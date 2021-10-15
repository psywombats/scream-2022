-- global defines for cutscenes

function teleportCoords(mapName, x, y)
    cs_teleportCoords(mapName, x, y)
    await()
end

function teleport(mapName, eventName)
    cs_teleport(mapName, eventName)
    await()
end

function fadeOutBGM(seconds)
    cs_fadeOutBGM(seconds)
    await()
end

function speak(speaker, line, eventTarget)
    cs_speak(speaker, line, eventTarget)
    await()
end

function speakP(portrait, line)
    cs_speakPortrait(portrait, line)
    await()
end

