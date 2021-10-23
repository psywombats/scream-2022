rotateTo('d5_owen')

if getSwitch('day5_01_owen') then
    speak("Owen", "Trust me. I can do it for sure.", 'd5_owen')
    return;
end

speak("Owen", "There!", 'd5_owen')
speak("TWIN", "Congrats!", 'd5_twin')
speak("Owen", "It only took three months of practice and I'm so much worse than before, but I played it all the way through.", 'd5_owen')
speak("TWIN", "That doesn't matter. To me, you sound incredible.", 'd5_twin')
speak("Owen", "Then just wait 'til I'm back to my old self. Trust me. I can do it for sure.", 'd5_owen')

setSwitch('day5_01_owen', true)
play('day5_outro')
