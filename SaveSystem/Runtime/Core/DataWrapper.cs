namespace SaveSystem
{
    /// <summary>
    /// DataWrapper Is Just An Serialize Wrapper Around The Given DataType....
    /// </summary>
    /// <typeparam name="T">type_of Data To Wrap...</typeparam>
    [System.Serializable]
    public struct DataWrapper<T>
    {
        /////////////////////////////////////////////////
        public T Data;

        /////////////////////////////////////////////////
        public DataWrapper(T data) => this.Data = data;
    }
}