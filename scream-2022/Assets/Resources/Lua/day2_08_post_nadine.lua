if getSwitch('day2_08_post_nadine') then
    speak("Nadine", "I keep flipping through my cards, but I can't see anything but Connie...", 'd2_nadine4')
    return
end

speak("Nadine", "Hey Tess.", 'd2_nadine4')
speak("Tess", "How are you doing?")
speak("Nadine", "I keep flipping through my cards, but I can't see anything but Connie.", 'd2_nadine4')
speak("Nadine", "What happened to her?", 'd2_nadine4')
speak("Tess", "She must've been sick.")
speak("Nadine", "With N9?", 'd2_nadine4')
speak("Tess", "There's no way to know.")

setSwitch('day2_08_post_nadine', true)
