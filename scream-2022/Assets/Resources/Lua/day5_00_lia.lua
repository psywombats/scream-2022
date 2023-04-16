rotateTo('d5_lia')

if getSwitch('day5_00_lia') then
    speak("Lia", "Thank you. I feel safe here.", 'd5_lia')
    return
end

speak("Lia", "I just wanted to say thank you, TWIN.", 'd5_lia')
speak("Lia", "I've... never really fit in anywhere.", 'd5_lia')
speak("Lia", "Since I found out I had caught N9, no one wanted anything to do with me.", 'd5_lia')
speak("TWIN", "You must've been brave to endure that.", 'd5_twin')
speak("Lia", "No, I wasn't. I'm a bit of a coward.", 'd5_lia')
speak("TWIN", "You don't have to worry. We'll look after you.", 'd5_twin')
speak("Lia", "I know you will. I feel safe here.", 'd5_lia')

setSwitch('day5_00_lia', true)
play('day5_outro')
