if getSwitch('day4_00_lia') then
    speak("Lia", "What happened to you, Cecily?", 'd4_lia')
    return
end

speak("Lia", "Cecily...", 'd4_lia')
speak("Lia", "What happened to you, Cecily?", 'd4_lia')
speak("Tess", "Lia? Hello?")
speak("Lia", "Just when I thought I'd fit in here...", 'd4_lia')
speak("Lia", "Where did you go?", 'd4_lia')
speak("Tess", "Lia. Read what I'm writing on the screen.", 'd4_lia')
speak("Lia", "...", 'd4_lia')

setSwitch('day4_00_lia', true)
