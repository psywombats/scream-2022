if getSwitch('day2_07_post_joey') then
    speak("Joey", "I'll never forget you, Connie.", 'd2_joey4')
    return
end

speak("Joey", "Connie...", 'd2_joey4')
speak("Tess", "I'm sorry. She was a good bunny.")
speak("Tess", "Maybe we can convince Dr. Cooper to find a new ward pet.")
faceTo('d2_joey4', 'hero')
speak("Joey", "You can't replace her.", 'd2_joey4')
speak("Tess", "No. You can't.")
speak("Tess", "She couldn't have asked for a better friend. Keep her memory with you.")
wait(.7)
speak("Joey", "I'll never forget you, Connie.", 'd2_joey4')

setSwitch('day2_07_post_joey', true)
