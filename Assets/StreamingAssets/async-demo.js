// test async works in both direction

module.exports.debugDrawLine = async function () {
    Vector3 = unity.UnityEngine.Vector3;
    Color = unity.UnityEngine.Color;
    const color = Color.green;
    for (let i = 0; i < 10000; i++) {
        const angle = i / 360.0;
        const x = Math.fround(Math.sin(angle) * 4);
        const y = Math.fround(Math.cos(angle) * 4);
        
        const position = new Vector3(x, y, 0.0);
        const position2 = new Vector3(-x, -y, 0.0);
        
        Debug.DrawLine(position, position2, color, Math.fround(1));
        await waitOneFrame();
    }
}
