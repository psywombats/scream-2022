if getSwitch('day3_03_joey') then
    speak("Joey", "Where are you...?", 'd3_joey0')
    return
end

speak("Joey", "...", 'd3_joey0')
speak("Lia", "Joey?", 'lia_bot')
speak("Joey", "...", 'd3_joey0')
speak("Lia", "Joey, it's me. Lia.", 'lia_bot')
face('d3_joey0', 'WEST')
speak("Joey", "Where are you...?", 'd3_joey0')
speak("Lia", "I'm right here, Joey.", 'lia_bot')
speak("Joey", "Where did you go...?", 'd3_joey0')
speak("Lia", "Okay Joey.", 'lia_bot')
speak("Joey", "Where...", 'd3_joey0')
face('d3_joey0', 'EAST')
wait(.8)
faceTo('hero', 'lia_bot')
speak("Lia", "T-this is Joey. His case is kind of pretty far along, but when he's all there, he's a good kid.", 'lia_bot')
face('hero', 'EAST')
speak("Lia", "Joey? Joey can you say hi to Tess?", 'lia_bot')
speak("Joey", "...", 'd3_joey0')
faceTo('hero', 'lia_bot')
speak("Tess", "Let's leave.")
wait(1.2)
speak("Joey", "Connie...", 'd3_joey0')

setSwitch('day3_03_joey', true)
play('day3_intro')
