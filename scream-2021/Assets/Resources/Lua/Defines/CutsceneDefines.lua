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

function intertitle(text)
    cs_intertitle(text)
    await()
end

function fade(fadeType)
    cs_fade(fadeType)
    await()
end

function search(text)
    cs_search(text)
    await()
end

function notebook(text)
    cs_notebook(text)
    await()
end

function flashcards()
    cs_flashcards()
    await()
end

function keywords(text)
    cs_keywords(text)
    await()
end

function choice(a, b)
    cs_choice(a, b)
    await()
end
