if getSwitch('day1_09_search_joey') then
    speak("Joey", "...", 'd1_joey2')
    return
end

speak("Tess", "Any luck?", 'd1_joey2')
speak("Joey", "There's no sign of her.", 'd1_joey2')
speak("Joey", "Why do you think she ran away?", 'd1_joey2')
speak("Tess", "Animals are like that.")
speak("Tess", "If they're cooped up in the same place all their lives, they naturally want to escape.")
speak("Joey", "...", 'd1_joey2')

setSwitch('day1_09_search_joey', true)
