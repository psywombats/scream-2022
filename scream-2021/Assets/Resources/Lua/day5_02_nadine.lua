rotateTo('d5_nadine')

if getSwitch('day5_02_nadine') then
    speak("Nadine", "Allen's finally coming to visit tomorrow!", 'd5_nadine')
    return
end

speak("Nadine", "TWIN! TWIN!", 'd5_nadine')
speak("TWIN", "What's up Nadine?", 'd5_twin')
speak("Nadine", "You won't believe it! Allen's finally coming to visit tomorrow!", 'd5_nadine')
speak("TWIN", "That's great news.", 'd5_twin')
speak("Nadine", "You wanna meet him? He's two years older than me, and really handsome! You'll like him!", 'd5_nadine')
speak("TWIN", "Haha, I wouldn't want to get in your way. You guys enjoy catching up.", 'd5_twin')

setSwitch('day5_02_nadine', true)
play('day5_outro')
