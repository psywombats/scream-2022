-- global defines for cutscenes

function teleportCoords(mapName, x, y)
    cs_teleportCoords(mapName, x, y)
    await()
end

function teleport(mapName, eventName, dir)
    cs_teleport(mapName, eventName, dir)
    await()
end

function targetTele(mapName, eventName, dir)
    cs_teleport(mapName, eventName, dir)
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

function card(val)
    cs_card(val)
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

function card(data)
    cs_card(data)
    await()
end

function caldeath(version)
    cs_caldeath(version)
    await()
end

function pathTo(eventName)
    cs_pathTo(eventName)
    await()
end

function walk(event, count, direction, wait)
    if wait == nil then wait = true end
    cs_walk(event, count, direction, wait)
    if wait then
        await()
    end
end

function pathEvent(mover, target, wait)
    if wait == nil then wait = true end
    cs_pathEvent(mover, target, wait)
    if wait then
        await()
    end
end

function rotateTo(target)
    cs_rotateTo(target)
    await()
end

