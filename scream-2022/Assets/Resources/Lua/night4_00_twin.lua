face('twin', 'SOUTH')
pathTo('twin_target')
rotateTo('twin')

speak("TWIN", "Hello. I see you've caught up to me.", 'twin')
speak("TWIN", "You're likely wondering who I am.", 'twin')
speak("TWIN", "Go ahead. You're in the driver's seat. Ask me my full name.", 'twin')
wait(.6)
speak("TWIN", "I'm waiting.", 'twin')
setDepthMult(.8)
wait(1)
speak("TWIN", "But of course. You can't speak.", 'twin')
speak("TWIN", "And you can't type, either, because I hold the screen right now.", 'twin')
speak("TWIN", "But that's okay, because I always know what you're thinking anyway.", 'twin')
face('twin', 'NORTH')
speak("TWIN", "You're wondering who I am, and why there are two of you.", 'twin')
face('twin', 'SOUTH')
setDepthMult(.7)
speak("TWIN", "But from my perspective, there is only one of me, and none of you.", 'twin')
speak("TWIN", "Which is why I will take over your role seamlessly.", 'twin')
wait(.8)
speak("TWIN", "It's a little sad.", 'twin')
wait(.4)
speak("TWIN", "Cal Cooper was really looking forward to meeting me, so, it's sort of a shame what happened with him.", 'twin')
speak("TWIN", "He was less misguided than you'd think.", 'twin')
face('twin', 'SOUTH')
speak("TWIN", "He just had the misfortune of mistaking you for me.", 'twin')
speak("TWIN", "But I know my true name. And my date of birth, the year, those details.", 'twin')
speak("TWIN", "You are just an incomplete version of me.", 'twin')
speak("TWIN", "There is also another critical difference between us.", 'twin')
pathEvent('twin', 'twin_target2')
face('twin', 'SOUTH')
wait(.5)

setDepthMult(.5)
playSFX('spoken_line')
wait(8)
setSwitch("spoken_lines", true)

setDepthMult(.75)
speak("TWIN", "I believe our identities are determined by our experiences.", 'twin')
speak("TWIN", "With none of your own, it's hard to say you really are anybody, or anyone.", 'twin')
speak("TWIN", "Or anything at all.", 'twin')
setDepthMult(.65)
wait(.6)
speak("TWIN", "I believe it's almost time for the final act, so we will hurry up.", 'twin')
speak("TWIN", "The matter of the True Name, though. I will leave that to you.", 'twin')
speak("TWIN", "What shall I call myself?", 'twin')

choice("Tess", "Cecily")
if choice_result == 0 then
    setSwitch('use_tess', true)
else
    setSwitch('use_cecily', true)
end

setDepthMult(.8)
speak("TWIN", "Sure.", 'twin')
speak("TWIN", "By the way, that is the only question you've answered that has affected anything.", 'twin')
speak("TWIN", "Consider it one last sign of your time here.", 'twin')
walk('twin', 1, 'NORTH')
face('twin', 'SOUTH')
wait(.4)
speak("TWIN", "Sadly, this is goodbye.", 'twin')
speak("TWIN", "I'm sure you'll be thinking a lot about me, but, I won't be thinking much about you.", 'twin')
setDepthMult(.4)
speak("TWIN", "In fact, I'm quite sure no one will think of you at all.", 'twin')

fade('black')
fadeOutBGM(1)
wait(1)
intertitle("I AM IMMORTAL\n\nI WILL LIVE ON THROUGH MY WORKS\nAND THROUGH MY FRIENDS AND THEIR MEMORIES\n\nWILL YOU?\n\n\nTWILIGHT")
setSwitch('d4_clear', true)
setSwitch('halla_lock', false)
teleport('Office1', 'target')