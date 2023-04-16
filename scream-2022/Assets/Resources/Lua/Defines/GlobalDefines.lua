-- global defines for coroutines mostly

function await ()
    coroutine.yield()
end

function wait(seconds)
    cs_wait(seconds)
    await()
end

function play(scenename)
    cs_play(scenename)
    await()
end
