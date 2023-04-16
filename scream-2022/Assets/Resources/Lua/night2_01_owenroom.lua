if not getSwitch('night2_01_owenroom') then
    rotateTo('target_piano')
    speak("No sign of Owen...")
    setSwitch('night2_01_owenroom', true)
end
