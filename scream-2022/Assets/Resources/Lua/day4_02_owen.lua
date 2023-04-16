if getSwitch('day4_02_owen') then
    speak("Owen", "Why do you not work, you stupid fingers?", 'd4_owen')
    return
end

speak("Owen", "That's not right at all! Damn it.", 'd4_owen')
speak("Owen", "Why do you not work, you stupid fingers?", 'd4_owen')
speak("Owen", "I can practice all day long and nothing will ever happen...", 'd4_owen')
wait(.8)
speak("Owen", "How do I get out of here? If I get out, who will I be?", 'd4_owen')

setSwitch('day4_02_owen', true)
