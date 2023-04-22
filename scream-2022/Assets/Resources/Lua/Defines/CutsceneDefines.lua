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

function speak(speaker, line)
    cs_speak(speaker, line)
    await()
end

function fade(fadeType)
    cs_fade(fadeType)
    await()
end

function enterNVL()
    cs_enterNVL()
    await()
end

function exitNVL()
    cs_exitNVL()
    await()
end

function rotateTo(event)
	cs_rotateTo(event)
	await()
end

function bootGazer(on)
	cs_bootGazer(on)
	await()
end

function enter(speaker, slot, alt)
    cs_enter(speaker, slot, alt)
    await()
end

function exit(speaker)
    cs_exit(speaker)
    await()
end

function expr(speaker, expression)
	cs_expr(speaker, expression)
	await()
end

function clue(item)
	cs_clue(item)
	await()
	return selection
end

function walk(event, count, direction, wait)
    if wait == nil then wait = true end
    cs_walk(event, count, direction, wait)
    if wait then
        await()
    end
end

