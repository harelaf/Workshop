using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public enum Operation
    {
        //REGISTERED
        OPEN_STORE, 
        REVIEW_PRODUCT, 
        RATE_PRODUCT_AND_STORE, 
        SEND_MESSAGE, 
        SEND_COMPLAINT, 
        PERSONAL_HISTIRY, 
        PERSONAL_INFO, 
        IMPROVE_SECURITY,

        //STORE OWNER
        MANAGE_INVENTORY,
        CHANGE_SHOP_AND_DISCOUNT_POLICY,
        DEFINE_CONCISTENCY_CONSTRAINT,      //only Store Founder
        APPOINT_OWNER,
        REMOVE_OWNER,
        APPOINT_MANAGER,
        REMOVE_MANAGER,
        CHANGE_MANAGER_PREMISSIONS, 
        CLOSE_STORE,                        //only Store Founder
        REOPEN_STORE,                       //only Store Founder
        STORE_WORKERS_INFO, 
        RECEIVE_AND_REPLY_STORE_MESSAGE, 
        STORE_HISTORY_INFO,                 //for System Admin as well

        //SYSTEM ADMIN
        PERMENENT_CLOSE_STORE,
        CANCEL_SUBSCRIPTION,
        RECEIVE_AND_REPLY_ADMIN_MESSAGE,
        SYSTEM_STATISTICS
    }
}
