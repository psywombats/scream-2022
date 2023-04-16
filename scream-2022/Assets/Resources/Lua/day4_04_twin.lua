if not getSwitch('day4_04_twin') then
    setSwitch('separating_camera', true)
    fadeOutBGM(1)
    pathTo('d4_target1')
    untrackCamera()
    face('hero', 'SOUTH')
    wait(1)
    setSwitch('d4_twin_appears', true)
    cs_resettleCamera()
    await()
    wait(1)
    
    walk('d4_twin0', 2, 'SOUTH')
    wait(.8)
    walk('d4_twin0', 3, 'EAST')
    face('d4_twin0', 'WEST')
    wait(.6)
    walk('d4_twin0', 4, 'EAST')
    
    setSwitch('d4_twin_appears', false)
    setSwitch('day4_04_twin', true)
    setSwitch('halla_lock', true)
    setSwitch('fp_only', true)
end
