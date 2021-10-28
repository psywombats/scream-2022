if getSwitch('day5_03_joey') then
    rotateTo('d5_twin')
    speak("TWIN", "Just make sure she doesn't get out again.", 'd5_twin')
    return
end

rotateTo('d5_joey')
speak("Joey", "C'mere Connie!", 'd5_joey')
speak("TWIN", "She'd probably like it if you gave her a treat. Maybe try some lettuce?", 'd5_twin')
speak("Joey", "I already fed her a ton after you rescued her. She must've been hungry!", 'd5_joey')
speak("TWIN", "Just make sure she doesn't get out again.", 'd5_twin')
speak("Joey", "I will! Thanks TWIN!", 'd5_joey')

setSwitch('day5_03_joey', true)
play('day5_outro')
