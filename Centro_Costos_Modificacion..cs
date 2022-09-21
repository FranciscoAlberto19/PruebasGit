----->>>>>>>><<<<<<"CENTROS DE COSTOS"<<<<<---->>>>>
        private bool ValidaUserAdmon (string perfil)
        {
            var CCAdministrados = ObtenerCentrosDeCostosAdministrados();
            return true;

           /* using (SqlConnection conn = new SqlConnection(connStr))
            {
                string cta;
                conn.Open();
                //string sql = "SELECT Count(*) AS Admon FROM usuariosCentroCosto UCC INNER JOIN usuarios U ON UCC.idUsuario =u.id " +
                //    " WHERE UCC.activo = 1 AND UCC.idCliente = @IdCliente AND idUsuario = @IdUsuario ";
                //if (perfil == "1")
                //{
                //    sql += " AND idN1 = @idN1 AND U.perfil = 9";
                //}
                //SqlCommand cmd = new SqlCommand(sql, conn);
                //cmd.Parameters.AddWithValue("@IdCliente", Session["idcliente"].ToString());
                //cmd.Parameters.AddWithValue("@IdUsuario", lblIdUsuario.Text);
                //cmd.Parameters.AddWithValue("@idN1", ddlN1.SelectedItem.Value);
                //SqlDataReader reader = cmd.ExecuteReader();
                //reader.Read();

                


                cta = "1"; //reader.GetValue(reader.GetOrdinal("Admon")).ToString();

                if (perfil == "1" && Convert.ToInt32(cta) >= 1)
                    return true;
                else if (Convert.ToInt32(cta) > 1)
                    return true;
                else
                    return false;
            }*/
        }

        /// <summary>
        /// 
        /// Key dictionary = Nivel del centro de costo
        /// Values dicitionary = Id de usuario que lo administra
        /// </summary>
        /// <returns></returns>
        private Dictionary<int,int> ObtenerCentrosDeCostosAdministrados()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                string sql = "SELECT UCC.*, U.perfil FROM usuariosCentroCosto UCC INNER JOIN usuarios U ON UCC.idUsuario =u.id WHERE UCC.idCliente = @IdCliente";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@IdCliente", Session["idcliente"].ToString());
                SqlDataReader reader = cmd.ExecuteReader();

                List<Dictionary<string, int>> CentCost = new List<Dictionary<string, int>>();
                //reader.Read();
                while (reader.Read())
                {
                    CentCost.Add(new Dictionary<string, int>
                    {
                        { "Nivel", ObtenerNivel(reader, 1)},
                        { "IdUsuario", (Int16)reader["idUsuario"] },
                        { "Act", ((bool)reader["activo"]) ? 1 : 0},
                        { "Perfil", (Int16)reader["perfil"]}

                    });
                    //reader.GetValue(reader.GetOrdinal("IdUsuario")).ToString();
                }


                Dictionary<int, int> CentCostAdministrados = new Dictionary<int, int>();
                CentCost.Where(item =>
                {
                    if (item["Act"] == 1 && item["Perfil"] == 9)
                        return true;
                    return false;
                }).ForEach(item => CentCostAdministrados.Add(item["Nivel"], item["IdUsuario"]));
                return CentCostAdministrados;
            }
        }
        
        private int ObtenerNivel(SqlDataReader reader, int nivel)
        {
            if (nivel > 10) return 0;
            if (reader[$"idN{nivel}"] is DBNull || (Int16)reader[$"idN{nivel}"] == 0) return 0;

            return 1 + ObtenerNivel(reader, nivel + 1);
        }
    }
