using System;

public class MapGenerationEventArgs : EventArgs {
    private Map map;

    public MapGenerationEventArgs(Map map) {
        this.map = map;
    }

    public Map Map => map;
}
