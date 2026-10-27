namespace Demonics
{
    public class Edge<T>
    {
        public T to;
        public int cost;

        public Edge(T to, int cost)
        {
            this.to = to;
            this.cost = cost;
        }

        public override bool Equals(object obj)
        {
            if(obj.GetType() != GetType()) return false;

            Edge<T> other = (Edge<T>)obj;

            if(other.to == null || !other.to.Equals(to)) return false;
            if(other.cost != cost) return false;

            return true;
        }
    }
}

