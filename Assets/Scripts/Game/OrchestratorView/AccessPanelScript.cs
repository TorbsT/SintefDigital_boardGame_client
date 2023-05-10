namespace Game.OrchestratorView
{
    public class AccessPanelScript : PanelScript
    {
        public override void handleInput()
        {
            int selectedIndex = (int) highlightedIndex;
            activeRegion.setAccessServer(selectedIndex);
            hidePanel();
        }
    }
}
